using DistributedPatientHealthCareSystem.DPHCSModels;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DistributedPatientHealthCareSystem.Hubs
{

    public class ChatHub : Hub
    {   //have user infromation
        //var name = Context.User.Identity.Name;
        //one user have multiple connections
        //ConnectionId = Context.ConnectionId;
        private DPHCSContext db = null;
        public ChatHub(DPHCSContext context)
        {
            db = context;
        }

        void Annouce(string message) {
            Clients.All.Announce("Hello");
        }

        public override Task OnConnected()
        {
            Clients.All.Announce("Hello");
            var myInfo = Context.QueryString["UserId"];
            var name = Context.User.Identity.Name;
            
                
                var delcon =db.UserConnection.Where(c=>c.UserName==myInfo.ToString());
                
                if (delcon.Count() != 0) {
                    db.UserConnection.RemoveRange(delcon);
                    db.SaveChanges();
                }
                       
                
                db.UserConnection.Add(new UserConnection
                {
                    ConnectionID = Context.ConnectionId,
                    UserName=myInfo
                });
                db.SaveChanges();
            
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            
              
                var connection = db.UserConnection.FirstOrDefault(u=>u.ConnectionID==Context.ConnectionId);
                if (connection!=null) {
                    db.Remove(connection);
                    db.SaveChanges();
                }
              

            
            return base.OnDisconnected(false);
        }





    }

}
