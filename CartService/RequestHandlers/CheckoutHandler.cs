using CartService.Data;
using CartService.Models.RequestModels;
using CartService.Models.ResponseModels;
using CartService.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CartService.RequestHandlers;

public class CheckoutHandler : IRequestHandler<CheckoutRequest, CheckoutResponse>
{
    private readonly CartDbContext _context;
    private readonly MailService _mailService;
    private readonly MidtransService _midtransService;

    public CheckoutHandler(CartDbContext context, MailService mailService, MidtransService midtransService)
    {
        _context = context;
        _mailService = mailService;
        _midtransService = midtransService;
    }

    public async Task<CheckoutResponse> Handle(CheckoutRequest request, CancellationToken cancellationToken)
    {
        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == request.UserId, cancellationToken);

        if (cart == null || !cart.Items.Any())
        {
            return new CheckoutResponse { Success = false, Message = "Cart is empty." };
        }

        decimal totalAmount = cart.Items.Sum(i => i.Price * i.Quantity);

        // ✅ Call Midtrans API
        var paymentUrl = await _midtransService.CreateTransactionAsync(
            cart.Id.ToString(),
            totalAmount,
            "customer@email.com"
        );

        if (paymentUrl == null)
            return new CheckoutResponse { Success = false, Message = "Failed to create payment transaction." };

        // ✅ Update status ke Pending, jangan hapus cart
        cart.Status = "Pending";
        await _context.SaveChangesAsync(cancellationToken);

        // ✅ Send confirmation email
        await _mailService.SendEmailAsync(
            "customer@email.com",
            "Checkout Confirmation",
            $"<h3>Thank you for your order!</h3><p>Order ID: {cart.Id}</p><p>Total: {totalAmount:C}</p>"
        );

        return new CheckoutResponse
        {
            Success = true,
            Message = "Checkout successful, please complete payment.",
            PaymentUrl = paymentUrl
        };
    }
}
