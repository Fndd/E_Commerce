using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
namespace E_Commerce.MvcWebUI.Entity
{
    public class DbContext
    { 
        IFirebaseConfig config;
       public IFirebaseClient client; 
        public DbContext()
        {

            config = new FirebaseConfig
            {
                AuthSecret = "NWpqL9Si00rIHKQav2p7UMjrzvcBpj0m8xAq0FWQ",
                BasePath = "https://ecommercemvcproject-default-rtdb.firebaseio.com/"

            };
           
            client = new FireSharp.FirebaseClient(config);
        }
    }
}
