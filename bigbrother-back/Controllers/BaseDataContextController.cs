using bigbrother_back.DataContext;
using bigbrother_back.Utility;

namespace bigbrother_back.Controllers
{
    public class BaseDataContextController : BaseController
    {
        #region Properties

        protected DatabaseContext DataModel { get; init; }

        protected ILogger<DebugController> Logger { get; init; }

        #endregion

        public BaseDataContextController(DatabaseContext dbContext,
                                         ILogger<DebugController> logger)
        {
            DataModel = dbContext;
            Logger = logger;
        }
    }
}
