using System.Net;
using System.Threading;

namespace Steam_Desktop_Authenticator
{
    class Post_AppStatus
    {


        public static string SendPostData_ToWebAppStatus(string SendToUrl, string AppNo)
        {
            // SendParameters = "param1=value1&param2=value2&param3=value3";
            string SendParameters = ""; // Parameters did not work for me so I didn't used it. My server app did not catch them, add added the parameters directly in the URL

            string HtmlResult = "Sending app status > Error";

            object value = null; // Used to store the return value
            var thread = new Thread(() => {
                try{
                    using (WebClient wc = new WebClient()) {
                        wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                        string PostData_GetReturnVal = wc.UploadString(SendToUrl + "?AppNo=" + AppNo + "&Status=ok", SendParameters); // return value from thread

                        if (PostData_GetReturnVal == "ok") {
                            value = "Sending app status > Done";
                        } else {
                            value = "Sending app status > Invalid Response";
                        }
                        
                    }
                }
                catch {
                    value = "Sending app status > Failed"; // return value from thread
                }
            });
            thread.Start();
            thread.Join();



            string ThreadValueReturn = value.ToString();
            if (ThreadValueReturn == null || ThreadValueReturn == "") { } else { HtmlResult = ThreadValueReturn; }

            return HtmlResult;
        }




    }
}