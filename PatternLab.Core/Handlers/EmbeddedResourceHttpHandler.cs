﻿using System.Web;
using System.Web.Routing;
using PatternLab.Core.Models;

namespace PatternLab.Core.Handlers
{
    public class EmbeddedResourceHttpHandler : IHttpHandler
    {
        private readonly RouteData _routeData;

        public EmbeddedResourceHttpHandler(RouteData routeData)
        {
            _routeData = routeData;
        }

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            var routeDataValues = _routeData.Values;
            var fileName = routeDataValues["file"].ToString();
            var virtualPath = string.Format("~/{0}/{1}/{2}", routeDataValues["root"], routeDataValues["folder"], fileName);

            var resource = new EmbeddedResource(virtualPath);
            using (var stream = resource.Open())
            {
                if (stream.Length <= 0) return;

                context.Response.Clear();
                context.Response.ContentType = MimeMapping.GetMimeMapping(fileName);

                stream.CopyTo(context.Response.OutputStream);
            }
        }
    }
}
