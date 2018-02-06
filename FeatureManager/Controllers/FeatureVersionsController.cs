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
        public IQueryable<FeatureVersion> GetFeatureVersions()
        {
            return db.FeatureVersions;
        }

        // GET: api/FeatureVersions/5
        [ResponseType(typeof(FeatureVersion))]
        public async Task<IHttpActionResult> GetFeatureVersion(int id)
        {
            FeatureVersion featureVersion = await db.FeatureVersions.FindAsync(id);
            if (featureVersion == null)
            {
                return NotFound();
            }

            return Ok(featureVersion);
        }

        // PUT: api/FeatureVersions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFeatureVersion(int id, FeatureVersion featureVersion)
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
                if (!FeatureVersionExists(id))
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
                if (FeatureVersionExists(featureVersion.VersionMajor))
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
        [ResponseType(typeof(FeatureVersion))]
        public async Task<IHttpActionResult> DeleteFeatureVersion(int id)
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

        private bool FeatureVersionExists(int id)
        {
            return db.FeatureVersions.Count(e => e.VersionMajor == id) > 0;
        }
    }
}