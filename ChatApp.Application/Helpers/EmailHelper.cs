namespace ChatApp.Application.Helpers
{
    public static class EmailHelper
    {
        public static async Task<string> GetUserActivationEmailSubject(string languageCode)
        {
            return languageCode switch
            {
                "tr" => @"barisclb.com için Email Adresi onayı",
                _ => @"Email Address Confirmation for barisclb.com"
            };
        }

        public static async Task<string> GetUserActivationEmailBody(string languageCode)
        {
            return languageCode switch
            {
                "tr" => @"
                            <div>
                               <h1>Merhaba {0}!</h1>
                               <p>Hesabını aktive etmek için gereken kod: '{1}'</p>
                               <p>Bu Email size Websitemizde hesap açılırken Email Adresiniz kullanıldığı için gönderildi. Eğer bunun bir hata olduğunu veya bir başkası tarafından kullanıldığını düşünüyorsanız, Websitemizden Email almayı bu linke tıklayarak durdurabilirsiniz: 
                                  <br>
                                  {2}{3}{4}{5}{6}
                               </p>
                            </div>",
                _ => @"
                       <div>
                           <h1>Hello {0}!</h1>
                           <p>Here is your Confirmation Code for Activating your Account: '{1}'</p>
                           <p>This Email was sent to you because your Email Address was used to Register on our Website. If you think this was a mistake or someone else used your Email Address to register, you can disable getting Emails from our website via this link: 
                              <br>
                              {2}{3}{4}{5}{6}
                           </p>
                        </div>"
            };
        }
    }
}
