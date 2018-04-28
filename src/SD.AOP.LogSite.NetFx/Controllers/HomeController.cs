using System.Configuration;
using System.Web.Mvc;

namespace SD.AOP.LogSite.Controllers
{
    /// <summary>
    /// 主页控制器
    /// </summary>
    public class HomeController : Controller
    {
        #region # 加载主页视图 —— ViewResult Index()
        /// <summary>
        /// 加载主页视图
        /// </summary>
        /// <returns>主页视图</returns>
        [HttpGet]
        public ViewResult Index()
        {
            this.ViewBag.WebName = ConfigurationManager.AppSettings["WebName"];
            this.ViewBag.TechSupport = ConfigurationManager.AppSettings["TechSupport"];

            return base.View();
        }
        #endregion
    }
}
