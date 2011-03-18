using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Just.Net.Protocols
{
    public enum SyslogFacility
    {
        KernelMessages = 0,
        UserLevelMessages,
        MailSystem,
        SystemDaemons,
        SecurityAuthorizationMessages,
        MessagesGeneratedInternallyBySyslogd,
        LinePrinterSubsystem,
        NetworkNewsSubsystem,
        UucpSubsystem,
        ClockDaemon,
        SecurityAuthorizationMessages2,
        FtpDaemon,
        NtpSubsystem,
        LogAudit,
        LogAlert,
        ClockDaemon2,
        Local0,
        Local1, 
        Local2,
        Local3,
        Local4,
        Local5,
        Local6,
        Local7
    }
}
