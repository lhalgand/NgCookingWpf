using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using apis.Data;
using apis.Models;

using Microsoft.AspNetCore.Mvc;

namespace apis.Controllers
{
    [Route("api/[controller]")]
    public class CommentairesController : Controller
    {
        IRepository<Commentaire> _commentairesRepository;

        public CommentairesController(IRepository<Commentaire> commentairesRepository)
        {
            _commentairesRepository = commentairesRepository;
        }

        [HttpPost]
        public void Post([FromBody]Commentaire comment)
        {
            _commentairesRepository.Add(comment);
        }
    }
}
