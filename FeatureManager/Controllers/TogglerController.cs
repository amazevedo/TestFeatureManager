using FeatureManager.DAL;
using FeatureManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FeatureManager.Controllers
{
    public class TogglerController : ApiController
    {
        private FeaturesContext db = new FeaturesContext();

        /// <summary>
        /// Endpoint to get features for a system
        /// </summary>
        /// <param name="SystemID">GUID of the system</param>
        /// <returns>List of features enabled for this System</returns>
        [HttpGet]
        [Route("api/Toggler/SystemFeatures/{SystemID}")]
        public IHttpActionResult SystemFeatures(Guid SystemID)
        {
            if (!db.Systems.Any(a => a.SystemIdentifier.Equals(SystemID)))
                return NotFound();

            return Ok(db.FeatureVersions.Where(w => w.IsEnabledIn.Any(a => a.SystemIdentifier.Equals(SystemID)))
                .GroupBy(g => g.FeatureID)
                .Select(s => s.OrderByDescending(o => o.VersionMajor)
                    .ThenByDescending(o => o.VersionMinor)
                    .ThenByDescending(o => o.VersionBuild)
                    .ThenByDescending(o => o.VersionRevision)
                    .FirstOrDefault())
                .ToList()
                .Select(s => new
                {
                    FeatureIdentifier = s.FeatureID,
                    Version = s.Version.ToString()
                }).ToList());
        }

        /// <summary>
        /// Function to set toggle values for a specific Feature
        /// </summary>
        /// <param name="FeatureID">Feature Identifyer</param>
        /// <returns>Status Code of the operation</returns>
        [HttpPost]
        public IHttpActionResult SetToggles(int FeatureID, string Version, bool isButtonBlue, bool isButtonGreen, bool isButtonRed, List<Guid> SystemList)
        {
            Version ver = new System.Version(Version);
            FeatureVersion featureVersion = db.FeatureVersions.Where(w => w.VersionMajor.Equals(ver.Major) && w.VersionMinor.Equals(ver.Minor) && w.VersionBuild.Equals(ver.Build) && w.VersionRevision.Equals(ver.Revision) && FeatureID.Equals(FeatureID)).FirstOrDefault();
            if (featureVersion == null)
            {
                return NotFound();
            }

            if (isButtonBlue)
            {
                featureVersion.IsEnabledIn = new List<Models.System>();
                featureVersion.IsDisabledIn = new List<Models.System>();
                db.SaveChanges();
                return Ok();
            }
            else
            {
                if (isButtonGreen)
                {
                    featureVersion.IsEnabledIn = db.Systems.Where(w => SystemList.Contains(w.SystemIdentifier)).ToList();
                    featureVersion.IsDisabledIn = new List<Models.System>();
                    db.SaveChanges();
                    return Ok();
                }
                else if (isButtonRed)
                {
                    featureVersion.IsEnabledIn = new List<Models.System>();
                    featureVersion.IsDisabledIn = db.Systems.Where(w => SystemList.Contains(w.SystemIdentifier)).ToList();
                    db.SaveChanges();
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
        }
    }
}
