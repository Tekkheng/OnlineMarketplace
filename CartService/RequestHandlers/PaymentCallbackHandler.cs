using CartService.Data;
using CartService.Models.ResponseModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CartService.RequestHandlers;

public class PaymentCallbackHandler : IRequestHandler<PaymentCallbackRequest, PaymentCallbackResponse>
{
    private readonly CartDbContext _context;

    public PaymentCallbackHandler(CartDbContext context)
    {
        _context = context;
    }

    public async Task<PaymentCallbackResponse> Handle(PaymentCallbackRequest request, CancellationToken cancellationToken)
    {
        var payload = request.Payload;

        var transactionStatus = payload.GetProperty("transaction_status").GetString();
        var orderId = payload.GetProperty("order_id").GetString();

        var cart = await _context.Carts
            .FirstOrDefaultAsync(c => c.Id.ToString() == orderId, cancellationToken);

        if (cart == null)
        {
            return new PaymentCallbackResponse
            {
                Success = false,
                Message = "Order not found",
                OrderId = orderId
            };
        }

        if (transactionStatus == "settlement" || transactionStatus == "capture")
        {
            cart.Status = "Paid";
        }
        else if (transactionStatus == "cancel" || transactionStatus == "deny" || transactionStatus == "expire")
        {
            cart.Status = "Failed";
        }
        else
        {
            cart.Status = "Pending";
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new PaymentCallbackResponse
        {
            Success = true,
            Message = $"Order {orderId} updated to {cart.Status}",
            OrderId = orderId,
            Status = cart.Status
        };
    }
}
