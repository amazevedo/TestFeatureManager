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
        public IQueryable<Feature> GetFeatures()
        {
            return db.Features;
        }

        // GET: api/ManagerFeatures/5
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