using System;
using System.Net;
using System.Threading;

namespace Steam_Desktop_Authenticator
{
    class Post_WebPlanner
    {


        public static string SendPostData_ToWebPlanner(string SendToUrl, string SteamId64, string AuthenticatorCode)
        {
            // SendParameters = "param1=value1&param2=value2&param3=value3";
            string SendParameters = ""; // Parameters did not work for me so I didn't used it. My server app did not catch them, add added the parameters directly in the URL

            string HtmlResult = "empty 1";

            object value = null; // Used to store the return value
            var thread = new Thread(() => {
                try {
                    using (WebClient wc = new WebClient()) {
                        wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                        string PostData_GetReturnVal = wc.UploadString(SendToUrl + "?Cmd=Planner&SteamId64=" + SteamId64 + "&Code=" + AuthenticatorCode, SendParameters); // return value from thread

                        if (PostData_GetReturnVal.IndexOf("SteamId64") > -1) { value = PostData_GetReturnVal;  } else { value = "empty 2"; }
                    }
                } catch { value = "Sending post > Failed"; } // return value from thread
            });
            thread.Start();
            thread.Join();
            
            string ThreadValueReturn = value.ToString();
            if (ThreadValueReturn == null || ThreadValueReturn == "") { HtmlResult = "empty 3";  } else { HtmlResult = ThreadValueReturn; }

            return HtmlResult;
        }


		public static string EncryptDecrypt_Shift(string DataStr, int Shift1, int Shift2, int Shift3, int Shift4, int Shift5)
        {
            string FinalString = ""; int CountShift = 0;
            foreach (char chr in DataStr)
            {
                string ch = chr.ToString();
                CountShift++; if (CountShift == 6) { CountShift = 1; }

                int ShiftBy = 0;
                if (CountShift == 1) { ShiftBy = Shift1; }
                else if (CountShift == 2) { ShiftBy = Shift2; }
                else if (CountShift == 3) { ShiftBy = Shift3; }
                else if (CountShift == 4) { ShiftBy = Shift4; }
                else if (CountShift == 5) { ShiftBy = Shift5; }

                int n;
                bool isNumeric = int.TryParse(ch.ToString(), out n);
                if(isNumeric == true){
                    string[] Arr1 = { "0","1","2","3","4","5","6","7","8","9","0","1","2","3","4","5","6","7","8","9","0","1","2","3","4","5","6","7","8","9" };
                    
                    int index = 0; int Ch_Found = 0; int Ch_FoundAtIndex = 0;
                    foreach (string Arr_Char in Arr1){
                        if(2 > Ch_Found){ // find in middle
                            if(Arr_Char == ch){ Ch_Found++; if(Ch_Found == 2){ Ch_FoundAtIndex = index; } }
                        } index++;
                    }

                    if(Ch_Found == 2){ int FinalShiftBy = Ch_FoundAtIndex + ShiftBy;
                      string NextChar = Arr1[FinalShiftBy];
                      FinalString += NextChar;
                    }else{ FinalString += ch; } // keep current character

                }else{  
                    string[] Arr1 = { "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z","a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z","a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z" };
                    
                    int ch_IsUppercase = 0;
                    if (ch == ch.ToLower()){ }else{ ch = ch.ToLower(); ch_IsUppercase = 1; }
                    
                    int index = 0; int Ch_Found = 0; int Ch_FoundAtIndex = 0;
                    foreach (string Arr_Char in Arr1){
                        if(2 > Ch_Found){ // find in middle
                            if(Arr_Char == ch){ Ch_Found++; if(Ch_Found == 2){ Ch_FoundAtIndex = index; } }
                        } index++;
                    }

                    if(Ch_Found == 2){ int FinalShiftBy = Ch_FoundAtIndex + ShiftBy;
                        string NextChar = Arr1[FinalShiftBy]; 
                        if( ch_IsUppercase == 1){ FinalString += NextChar.ToUpper(); }else{ FinalString += NextChar; }
                    }else{  // keep current character
                        if( ch_IsUppercase == 1){ FinalString += ch.ToUpper(); }else{ FinalString += ch; }
                    }
                }
            }
            return FinalString;
        }





    }
}