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
    public class ManagerFeaturesController : ApiController
    {
        private FeaturesContext db = new FeaturesContext();

        // GET: api/ManagerFeatures
        /// <summary>
        /// List Registered Features
        /// </summary>
        /// <returns>List of all Features</returns>
        [Authorize]
        public IQueryable<Feature> GetFeatures()
        {
            return db.Features;
        }

        // GET: api/ManagerFeatures/5
        /// <summary>
        /// Getting a single Feature
        /// </summary>
        /// <param name="id">Feature ID</param>
        /// <returns>Single Feature</returns>
        [Authorize]
        [ResponseType(typeof(Feature))]
        public async Task<IHttpActionResult> GetFeature(int id)
        {
            Feature feature = await db.Features.FindAsync(id);
            if (feature == null)
            {
                return NotFound();
            }

            return Ok(feature);
        }

        // PUT: api/ManagerFeatures/5
        /// <summary>
        /// Update one Feature
        /// </summary>
        /// <param name="id">Feature ID</param>
        /// <param name="feature">New Feature to update</param>
        /// <returns>StatusCode of the operation</returns>
        [Authorize]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFeature(int id, Feature feature)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != feature.FeatureId)
            {
                return BadRequest();
            }

            db.Entry(feature).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeatureExists(id))
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

        // POST: api/ManagerFeatures
        /// <summary>
        /// Add new Feature
        /// </summary>
        /// <param name="feature">New Feature to add</param>
        /// <returns>Feature added</returns>
        [Authorize]
        [ResponseType(typeof(Feature))]
        public async Task<IHttpActionResult> PostFeature(Feature feature)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Features.Add(feature);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = feature.FeatureId }, feature);
        }

        // DELETE: api/ManagerFeatures/5
        /// <summary>
        /// Delete Feature
        /// </summary>
        /// <param name="id">Feature ID to be delete</param>
        /// <returns>Status Code of the operation</returns>
        [Authorize]
        [ResponseType(typeof(Feature))]
        public async Task<IHttpActionResult> DeleteFeature(int id)
        {
            Feature feature = await db.Features.FindAsync(id);
            if (feature == null)
            {
                return NotFound();
            }

            db.Features.Remove(feature);
            await db.SaveChangesAsync();

            return Ok(feature);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FeatureExists(int id)
        {
            return db.Features.Count(e => e.FeatureId == id) > 0;
        }
    }
}