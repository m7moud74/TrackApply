public class EmailBackGroundService(NotificationCahnnel cahnnel,ILogger<EmailBackGroundService> logger): BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Email Background Service is starting...");

        await foreach (var @event in cahnnel.ReadAllAsync(stoppingToken))
        {
            logger.LogInformation("Sending email to {Email}... Your application {Id} is now {Status}", 
                @event.UserEmail, @event.JobApplicationId, @event.NewStatus);
            
            //محاكاة وهمية لعملية إرسال إيميل بتاخد ثانيتين
            await Task.Delay(2000, stoppingToken); 
            
            logger.LogInformation("Email sent successfully to {Email}!", @event.UserEmail);
        }
    }
}