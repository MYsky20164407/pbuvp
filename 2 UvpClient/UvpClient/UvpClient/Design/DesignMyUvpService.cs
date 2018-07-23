using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UvpClient.Models;
using UvpClient.Services;

namespace UvpClient.Design {
    public class DesignMyUvpService : IMyUvpService {
        private readonly DesignDataContext _designDataContext;

        public DesignMyUvpService(DesignDataContext designDataContext) {
            _designDataContext = designDataContext;
        }

        public async Task<ServiceResult<MyUvp>> GetAsync() {
            return new ServiceResult<MyUvp> {
                Status = ServiceResultStatus.OK,
                Result = new MyUvp {
                    Announcements = _designDataContext.Announcement.ToList(),
                    StudentAssignments = _designDataContext.StudentAssignment
                        .Include(m => m.Homework).ToList(),
                    GroupAssignments = _designDataContext.GroupAssignment
                        .Include(m => m.Homework).ToList(),
                    Votes = _designDataContext.Vote
                        .Include(m => m.Questionnaire).ToList(),
                    PeerWorkGroupEvaluations = _designDataContext
                        .PeerWorkGroupEvaluation.ToList()
                }
            };
        }
    }
}