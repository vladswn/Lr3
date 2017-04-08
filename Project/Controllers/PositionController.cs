using Project.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Project.Controllers
{
    public class PositionController : ApiController
    {
        ApplicationDbContext context = new ApplicationDbContext();

        // GET Admin/api/Position
        [HttpGet]
        public IEnumerable<Position> GetPositions()
        {
            return context.Positions.ToList();
        }

        // POST Admin/api/values
        [HttpPost]
        public IHttpActionResult PostPosition(Position position)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            context.Positions.Add(position);
            context.SaveChanges();
            return CreatedAtRoute("DefaultApi", new { id = position.PositionId}, position);
        }


        // GET Admin/api/values/id
        [HttpGet]
        public IHttpActionResult GetPosition(int id)
        {
            var position = context.Positions.Find(id);
            if (position == null)
            {
                return NotFound();
            }

            return Ok(position);
        }

        // PUT Admin/api/values/id
        [HttpPut]
        public IHttpActionResult PutPosition(int id, Position position)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != position.PositionId)
            {
                return BadRequest();
            }

            context.Entry(position).State = EntityState.Modified;

            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PositionExists(id))
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

        // POST Admin/api/values/id
        [HttpDelete]
        public IHttpActionResult DeletePosition(int id)
        {
            var position = context.Positions.Find(id);
            if (position == null)
            {
                return NotFound();
            }

            context.Positions.Remove(position);
            context.SaveChanges();

            return Ok(position);
        }

        private bool PositionExists(int id)
        {
            return context.Positions.Count(e => e.PositionId == id) > 0;
        }
    }
}
