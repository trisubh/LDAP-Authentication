using System;

namespace ValidateLDAP
{
    class Program
    {
        static void Main(string[] args)
        {
            string LDAP = "LDAP://corp.sagitec.com/DC=corp,DC=sagitec,DC=com";

            User user = new User("saurabh.Tripathi", "Sky#2021");

            new BL().CheckUserInAD(LDAP, ref user);

            Console.WriteLine("User ID:"+user.Userid+ "\n" +"First Name:"+user.FirstName+"\n"+ "Last Name:"+user.LastName+"\n"+ "Email:"+user.Email+"\n"+ "Message:"+user.Message );
            Console.WriteLine("################## User Context ################");

        }


    }


}
