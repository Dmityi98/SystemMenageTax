using MassTransit;
using Microsoft.EntityFrameworkCore;
using SMT.Application;
using SMT.Application.Interfaces;
using SMT.Events.Events;

namespace SMT.Application.Consumers;

public class PaymentConsumer(
    IPublishEndpoint _publicEndpoint,
    ISMTDBContext _context) : IConsumer<CreateSuccessPayment>
{
    public async Task Consume(ConsumeContext<CreateSuccessPayment> context)
    {
        var message = context.Message;
        var ct = context.CancellationToken;
        Console.WriteLine(message.ToString());
        var user = await _context.Users.
            Include(u=> u.Profile).
            FirstOrDefaultAsync(u => u.Id == message.UserId, ct);

        if (user != null)
        {
            user.IsActive = true;
            user.Profile.EndDatePayment = context.Message.Date;
            await _context.SaveChangesAsync(ct);
            return;
        }
    }
    
}