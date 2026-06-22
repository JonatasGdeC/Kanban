using Kanban.Communication.Email.Resource;

namespace Kanban.Communication.Email;

public static class ResetPasswordTemplate
{
    public static string Execute(string username, string code)
    {
        return $$"""
                 <!DOCTYPE html>
                 <html lang="pt-BR">
                 <head>
                   <meta charset="UTF-8" />
                   <meta name="viewport" content="width=device-width, initial-scale=1.0" />
                   <title>{{ResourceEmailMessage.TITLE_RESET_PASSWORD}}</title>
                   <style>
                     * { margin: 0; padding: 0; box-sizing: border-box; }
                 
                     body {
                       background-color: #F4F7FD;
                       font-family: 'Plus Jakarta Sans', Arial, sans-serif;
                       color: #000112;
                       padding: 40px 16px;
                     }
                 
                     .email-wrapper {
                       max-width: 520px;
                       margin: 0 auto;
                     }
                 
                     /* Header */
                     .email-header {
                       display: flex;
                       align-items: center;
                       gap: 12px;
                       margin-bottom: 32px;
                       padding: 0 4px;
                     }
                 
                     .logo-icon {
                       display: flex;
                       align-items: center;
                       gap: 4px;
                     }
                 
                     .logo-bar {
                       width: 6px;
                       border-radius: 3px;
                     }
                 
                     .logo-bar--1 { height: 26px; background-color: #635FC7; }
                     .logo-bar--2 { height: 20px; background-color: #A8A4FF; }
                     .logo-bar--3 { height: 14px; background-color: #E4EBFA; }
                 
                     .logo-text {
                       font-size: 22px;
                       font-weight: 800;
                       color: #000112;
                     }
                 
                     /* Card */
                     .email-card {
                       background-color: #FFFFFF;
                       border-radius: 6px;
                       padding: 40px;
                       box-shadow: 0 8px 24px rgba(54, 78, 126, 0.08);
                     }
                 
                     .email-accent {
                       font-size: 11px;
                       font-weight: 700;
                       letter-spacing: 2.5px;
                       text-transform: uppercase;
                       color: #635FC7;
                       margin-bottom: 12px;
                     }
                 
                     .email-title {
                       font-size: 22px;
                       font-weight: 800;
                       color: #000112;
                       line-height: 1.3;
                       margin-bottom: 20px;
                     }
                 
                     .email-body {
                       font-size: 14px;
                       font-weight: 500;
                       color: #828FA3;
                       line-height: 1.65;
                       margin-bottom: 16px;
                     }
                 
                     /* Bloco do código */
                     .code-block {
                       margin: 28px 0;
                       background-color: #F4F7FD;
                       border-radius: 6px;
                       padding: 28px 24px;
                       text-align: center;
                     }
                 
                     .code-label {
                       font-size: 11px;
                       font-weight: 700;
                       letter-spacing: 2px;
                       text-transform: uppercase;
                       color: #828FA3;
                       margin-bottom: 16px;
                     }
                 
                     .code-value {
                         background-color: #FFFFFF;
                         border-radius: 6px;
                         border: 2px solid #E4EBFA;
                         text-align: center;
                         font-size: 24px;
                         font-weight: 800;
                         color: #635FC7;
                         box-shadow: 0 2px 6px rgba(54, 78, 126, 0.08);
                         padding: 12px 24px;
                     }
                 
                     .code-expiry {
                       margin-top: 16px;
                       font-size: 12px;
                       color: #828FA3;
                       font-weight: 500;
                     }
                 
                     .code-expiry strong {
                       color: #EA5555;
                     }
                 
                     /* Alerta vermelho */
                     .email-alert {
                       font-size: 12px;
                       font-weight: 500;
                       color: #EA5555;
                       padding: 10px 12px;
                       background-color: rgba(234, 85, 85, 0.08);
                       border-radius: 4px;
                       border-left: 3px solid #EA5555;
                       margin-bottom: 16px;
                       line-height: 1.5;
                     }
                 
                     /* Divider */
                     .email-divider {
                       border: none;
                       border-top: 1px solid #E4EBFA;
                       margin: 32px 0;
                     }
                 
                     .email-note {
                       font-size: 12px;
                       color: #828FA3;
                       line-height: 1.6;
                     }
                 
                     /* Footer */
                     .email-footer {
                       text-align: center;
                       margin-top: 24px;
                       font-size: 12px;
                       color: #828FA3;
                     }
                   </style>
                 </head>
                 <body>
                   <div class="email-wrapper">
                 
                     <div class="email-header">
                       <div class="logo-icon">
                         <div class="logo-bar logo-bar--1"></div>
                         <div class="logo-bar logo-bar--2"></div>
                         <div class="logo-bar logo-bar--3"></div>
                       </div>
                       <span class="logo-text">kanban</span>
                     </div>
                 
                     <div class="email-card">
                       <p class="email-accent">{{ResourceEmailMessage.RESET_ACCENT}}</p>
                       <h1 class="email-title">{{string.Format(format: ResourceEmailMessage.RESET_HEADING, arg0: username.Split(separator: " ")[0])}}</h1>

                       <p class="email-body">{{ResourceEmailMessage.RESET_BODY_1}}</p>

                       <div class="code-block">
                         <p class="code-label">{{ResourceEmailMessage.RESET_CODE_LABEL}}</p>
                         <p class="code-value">{{code}}</p>
                         <p class="code-expiry">{{ResourceEmailMessage.RESET_CODE_EXPIRY}}</p>
                       </div>

                       <div class="email-alert">{{ResourceEmailMessage.RESET_ALERT}}</div>

                       <p class="email-body">{{ResourceEmailMessage.RESET_BODY_2}}</p>

                       <hr class="email-divider" />

                       <p class="email-note">{{ResourceEmailMessage.RESET_NOTE}}</p>
                     </div>
                 
                     <div class="email-footer">
                       <p>Kanban App &mdash; {{ResourceEmailMessage.STUDY_PROJECT}} <a href="https://jonatasgdec-portifolio.vercel.app/" target="_blank" style="color: #000; text-decoration: underline; font-weight: 600;">JonatasGdeC</a> &copy; {{DateTime.Now.Year}}</p>
                     </div>
                 
                   </div>
                 </body>
                 </html>
                 """;
    }
}