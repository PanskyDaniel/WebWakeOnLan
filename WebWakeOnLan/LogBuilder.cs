using Karambolo.Extensions.Logging.File;
using System.Text;

namespace WebWakeOnLan
{
    public class LogBuilder: FileLogEntryTextBuilder
    {
        public override void BuildEntryText(StringBuilder sb, string categoryName, LogLevel logLevel, EventId eventId, string message, Exception exception, IExternalScopeProvider scopeProvider, DateTimeOffset timestamp)
        {
            sb.Append(timestamp.ToLocalTime().ToString("dd.MM.yyyy HH:mm:ss"));
            sb.Append(" ");

            AppendCategoryName(sb, categoryName);
            sb.Append(" ");

            AppendLogLevel(sb, logLevel);
            sb.AppendLine();

            if (!string.IsNullOrEmpty(message))
                AppendMessage(sb, message);

            if (exception != null)
                AppendException(sb, exception);

            sb.AppendLine();

        }
    }
}
