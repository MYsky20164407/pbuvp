using System;
using System.Threading.Tasks;
using UvpClient.Services;

namespace UvpClient.Design {
    public class DesignIdentityService : IIdentityService {
        public IdentifiedHttpMessageHandler GetIdentifiedHttpMessageHandler() {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult> LoginAsync() {
            throw new NotImplementedException();
        }

        public void Save() {
            throw new NotImplementedException();
        }
    }
}