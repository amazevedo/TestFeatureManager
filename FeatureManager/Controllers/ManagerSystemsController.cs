using FeatureManager.DAL;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace FeatureManager.Controllers
{
    public class ManagerSystemsController : ApiController
    {
        private FeaturesContext db = new FeaturesContext();

        // GET: api/ManagerSystems
        public IQueryable<Models.System> GetSystems()
        {
            return db.Systems;
        }

        // GET: api/ManagerSystems/5
        [ResponseType(typeof(Models.System))]
        public async Task<IHttpActionResult> GetSystem(Guid id)
        {
            Models.System system = await db.Systems.FindAsync(id);
            if (system == null)
            {
                return NotFound();
            }

            return Ok(system);
        }

        // PUT: api/ManagerSystems/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSystem(Guid id, Models.System system)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != system.SystemIdentifier)
            {
                return BadRequest();
            }

            db.Entry(system).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SystemExists(id))
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

        // POST: api/ManagerSystems
        [ResponseType(typeof(Models.System))]
        public async Task<IHttpActionResult> PostSystem(Models.System system)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Systems.Add(system);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SystemExists(system.SystemIdentifier))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = system.SystemIdentifier }, system);
        }

        // DELETE: api/ManagerSystems/5
        [ResponseType(typeof(Models.System))]
        public async Task<IHttpActionResult> DeleteSystem(Guid id)
        {
            Models.System system = await db.Systems.FindAsync(id);
            if (system == null)
            {
                return NotFound();
            }

            db.Systems.Remove(system);
            await db.SaveChangesAsync();

            return Ok(system);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SystemExists(Guid id)
        {
            return db.Systems.Count(e => e.SystemIdentifier == id) > 0;
        }
    }
}