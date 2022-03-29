using Firebase.Database;
using Firebase.Database.Query;

namespace E_Commerce.MvcWebUI.Managing
{
    public class DbContext
    {
        public FirebaseClient firebaseClient;
        public DbContext()
        { 
             firebaseClient = new FirebaseClient("https://ecommercemvcproject-default-rtdb.firebaseio.com/"); 
        }
    }
}
