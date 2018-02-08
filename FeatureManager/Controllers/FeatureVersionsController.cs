using FeatureManager.DAL;
using FeatureManager.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace FeatureManager.Controllers
{
    public class FeatureVersionsController : ApiController
    {
        private FeaturesContext db = new FeaturesContext();

        // GET: api/FeatureVersions
        /// <summary>
        /// List Registered Features Versions
        /// </summary>
        /// <returns>List of all Features Versions</returns>
        [Authorize]
        public IQueryable<FeatureVersion> GetFeatureVersions()
        {
            return db.FeatureVersions;
        }

        // GET: api/FeatureVersions/5/1.0.0
        /// <summary>
        /// Get a especific Feature Version
        /// </summary>
        /// <param name="id">Feature ID</param>
        /// <param name="version">Version Number</param>
        /// <returns>Single Single Feature Version</returns>
        [Authorize]
        [ResponseType(typeof(FeatureVersion))]
        public async Task<IHttpActionResult> GetFeatureVersion(int id, string version)
        {
            System.Version ver = new System.Version(version);
            FeatureVersion featureVersion = db.FeatureVersions.Where(w => w.VersionMajor.Equals(ver.Major) && w.VersionMinor.Equals(ver.Minor) && w.VersionBuild.Equals(ver.Build) && w.VersionRevision.Equals(ver.Revision) && w.FeatureID.Equals(id)).FirstOrDefault();

            if (featureVersion == null)
            {
                return NotFound();
            }

            return Ok(featureVersion);
        }

        // PUT: api/FeatureVersions/5
        /// <summary>
        /// Update Feature Version
        /// </summary>
        /// <param name="id">Feature Version ID</param>
        /// <param name="version">Version Number</param>
        /// <param name="featureVersion">New Feature Version to update</param>
        /// <returns>StatusCode of the operation</returns>
        [Authorize]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFeatureVersion(int id, string version, FeatureVersion featureVersion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != featureVersion.VersionMajor)
            {
                return BadRequest();
            }

            db.Entry(featureVersion).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeatureVersionExists(id, version))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/FeatureVersions
        /// <summary>
        /// Add new Feature Version
        /// </summary>
        /// <param name="featureVersion">New Feature Version to add</param>
        /// <returns>Feature Version added</returns>
        [Authorize]
        [ResponseType(typeof(FeatureVersion))]
        public async Task<IHttpActionResult> PostFeatureVersion(FeatureVersion featureVersion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.FeatureVersions.Add(featureVersion);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (FeatureVersionExists(featureVersion.FeatureID, featureVersion.Version.ToString()))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = featureVersion.VersionMajor }, featureVersion);
        }

        // DELETE: api/FeatureVersions/5
        /// <summary>
        /// Delete Feature Version
        /// </summary>
        /// <param name="id">Feature Version ID</param>
        /// <param name="version">Version Number</param>
        /// <returns>Status Code of the operation</returns>
        [Authorize]
        [ResponseType(typeof(FeatureVersion))]
        public async Task<IHttpActionResult> DeleteFeatureVersion(int id, string version)
        {
            FeatureVersion featureVersion = await db.FeatureVersions.FindAsync(id);
            if (featureVersion == null)
            {
                return NotFound();
            }

            db.FeatureVersions.Remove(featureVersion);
            await db.SaveChangesAsync();

            return Ok(featureVersion);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FeatureVersionExists(int id, string version)
        {
            System.Version ver = new System.Version(version);

            return db.FeatureVersions.Where(w => w.VersionMajor.Equals(ver.Major) && w.VersionMinor.Equals(ver.Minor) && w.VersionBuild.Equals(ver.Build) && w.VersionRevision.Equals(ver.Revision) && w.FeatureID.Equals(id)).Count() > 0;
        }
    }
}