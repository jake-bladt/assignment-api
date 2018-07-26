using System;
using System.Diagnostics;
using System.Linq;

using Microsoft.AspNetCore.Mvc;

using JakeBladt.AssignmentAPI.Data;
using JakeBladt.AssignmentAPI.Domain;
using JakeBladt.AssignmentAPI.ViewModels;

namespace JakeBladt.AssignmentAPI.Controllers
{
    [Route("api/v0.1/[controller]")]
    public class AssignmentsController : Controller
    {
        protected ApiDbContext _context;

        public AssignmentsController(ApiDbContext context)
        {
            _context = context;
        }

        /**
         * @api {get} /assignments/?tag=:tag
         * @apiNmae GetAssignmentsByTag
         * @apiGroup Assignment
         * 
         * @apiParam {String} tag Tag on assignment
         * 
         * @apiSuccess {String} name Name of the assignment.
         * @apiSuccess {String} title Title of the assignment.
         * @apiSuccess {String} description Description of the assignment.
         * @apiSuccess {String} type Type of the assignment.
         * @apiSuccess {[String]} tags Tags on the assignment.
        */
        [HttpGet]
        public IActionResult Get(string tag)
        {
            // The only requirement for this endpoint is that it be able to get multiple assignments by
            // tag. All non-tag queries respond with a 404.
            if (String.IsNullOrEmpty(tag)) return NotFound();

            try
            {
                var assignments = from a in _context.AssignmentsByTag(tag) select AssignmentViewmodel.FromAssignment(a);
                return Ok(assignments);
            }
            catch
            {
                return StatusCode(500);
            }

        }

        /**
         * @api {get} /assignments/:id
         * @apiNmae GetAssignmentById
         * @apiGroup Assignment
         * 
         * @apiParam {Number} id ID of the assignment
         * 
         * @apiSuccess {String} name Name of the assignment.
         * @apiSuccess {String} title Title of the assignment.
         * @apiSuccess {String} description Description of the assignment.
         * @apiSuccess {String} type Type of the assignment.
         * @apiSuccess {[String]} tags Tags on the assignment.
        */
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var assignment = _context.Assignments.FirstOrDefault(a => a.AssignmentId == id);
            if (null == assignment)
            {
                return NotFound();
            }
            else
            {
                var vm = AssignmentViewmodel.FromAssignment(assignment);
                return Ok(vm);
            }
        }

        /**
         * @api {post} /assignments/
         * @apiNmae CreateAssignment
         * @apiGroup Assignment
         * 
         * @apiParam {String} name Name of the assignment.
         * @apiParam {String} title Title of the assignment.
         * @apiParam {String} description Description of the assignment.
         * @apiParam {String} type Type of the assignment.
         * @apiParam {[String]} tags Tags on the assignment.
         * 
         * @apiSuccees {String} id ID of the assignment
        */
        [HttpPost]
        public IActionResult Post(string name, string assignmentType, string title, string description, string duration, string tags)
        {
            var tagList = string.IsNullOrEmpty(tags) ? new string[0] : tags.Split(",");

            var newAssignment = new Assignment
            {
                Name = name,
                AssignmentType = assignmentType,
                Title = title,
                Description = description,
                Duration = duration
            };

            try
            {
                _context.Assignments.Add(newAssignment);
                _context.SaveChanges();

                if (tagList.Length > 0)
                {
                    for (int i = 0; i < tagList.Length; i++)
                    {
                        var tag = _context.Tags.FirstOrDefault(t => t.Name == tagList[i]);
                        if (null == tag)
                        {
                            tag = new Tag { Name = tagList[i] };
                            _context.Tags.Add(tag);
                            _context.SaveChanges();
                        }

                        var link = new AssignmentTag
                        {
                            Assignment = newAssignment,
                            AssignmentId = newAssignment.AssignmentId,
                            Tag = tag,
                            TagId = tag.TagId
                        };
                        newAssignment.AssignmentTags.Add(link);
                    }
                }

                _context.SaveChanges();
                var vm = AssignmentViewmodel.FromAssignment(newAssignment);
                vm.Tags = tagList;
                return Ok(vm);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                return StatusCode(500);
            }

        }
    }
}
