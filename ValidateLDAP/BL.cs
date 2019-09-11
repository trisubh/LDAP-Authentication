using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Text;

namespace ValidateLDAP
{
    class BL
    {
        public Dictionary<string, string> LDAPContext(string LDAP)
        {
            string ADSpath = LDAP;
            ADSpath = ADSpath.Replace("LDAP://", "");
            Dictionary<string, string> domaincontext = new Dictionary<string, string>();
            if (ADSpath.Split('/').Length > 1)
            {
                domaincontext.Add("host", ADSpath.Split('/')[0].Replace("LDAP://", ""));
                domaincontext.Add("container", ADSpath.Split('/')[1]);
            }
            else
            {
                domaincontext.Add("host", ADSpath.Split('/')[0].Replace("LDAP://", ""));
            }
            return domaincontext;
        }

        public PrincipalContext GetPrincipalContextLDAP(string LDAPString, string uid, string pwd)
        {
            string contexthost = string.Empty;
            string contextcontainer = string.Empty;
            PrincipalContext lobjPrincipalContext = new PrincipalContext(ContextType.Domain);
            var ctx = LDAPContext(LDAPString);
            if (ctx.TryGetValue("host", out contexthost))
            {
                if (ctx.TryGetValue("container", out contextcontainer))
                {
                    lobjPrincipalContext = new PrincipalContext(ContextType.Domain, contexthost, contextcontainer, uid, pwd);
                }
                else
                {
                    lobjPrincipalContext = new PrincipalContext(ContextType.Domain, contexthost, null, uid, pwd);
                }

            }
            return lobjPrincipalContext;
        }


        public void CheckUserInAD(string istrAdsPath,ref User user)
        {
            istrAdsPath = istrAdsPath.ToUpper();
            try
            {
                PrincipalContext lobjPrincipalContext = GetPrincipalContextLDAP(istrAdsPath,user.Userid,user.Password);

                Console.WriteLine("Authentication in progress..");
                Console.WriteLine("################## User Context ################");
                UserPrincipal lobjUserPrincipal = UserPrincipal.FindByIdentity(lobjPrincipalContext, user.Userid);

                // User entry not found
                if (lobjUserPrincipal == null)
                {
                    user.Message = "ERROR-AD001: User entry not found in Active Directory. Check ADS path.";
                }

                // User account is locked
                if (lobjUserPrincipal.IsAccountLockedOut())
                {
                    user.Message = "ERROR-AD002: User account is locked in Active Directory.";
                }

                // Validate the user
                if (!lobjPrincipalContext.ValidateCredentials(user.Userid, user.Password))
                {
                    user.Message = "ERROR-AD003: Cannot authenticate user. Invalid Username or Password";
                }

                //thunmbnail photo
                //DirectoryEntry etry = (DirectoryEntry)lobjUserPrincipal.GetUnderlyingObject();
                //PropertyValueCollection col = etry.Properties["thumbnailPhoto"];
                //if (col.Value != null && col.Value is byte[])
                //{
                //    byte[] thumbnailInBytes = (byte[])col.Value;
                //     //new Bitmap(new MemoryStream(thumbnailInBytes));
                //}
            

                // Set user information
                user.FirstName = lobjUserPrincipal.GivenName;
                user.LastName = lobjUserPrincipal.Surname;
                user.Email = lobjUserPrincipal.EmailAddress;
                user.PhoneNumber = lobjUserPrincipal.VoiceTelephoneNumber;

                // Set user status
                user.Message = "Successful Authentication";
            }
            catch (MultipleMatchesException)
            {
                user.Message = "ERROR-AD004: Multiple user entries found in Active Directory. Check ADS path.";
            }
            catch (Exception ex)
            {
                user.Message = "ERROR-AD005: Cannot authenticate user. " + ex.Message;
            }
            finally
            {
            }
        }
    }
}
