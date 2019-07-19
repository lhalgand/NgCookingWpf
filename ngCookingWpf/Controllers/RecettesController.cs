﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using apis.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using apis.Data;

namespace apis.Controllers
{
    [Route("api/[controller]")]
    public class RecettesController : Controller
    {
        IRepository<Recette> _recettesRepository;
        private readonly IHostingEnvironment _appEnvironment;
        IRepository<Ingredient> _ingredientsRepository;

        public RecettesController(IRepository<Recette> recettesRepository, IRepository<Ingredient> ingredientsRepository, IHostingEnvironment appEnvironment)
        {
            _recettesRepository = recettesRepository;
            _ingredientsRepository = ingredientsRepository;
            _appEnvironment = appEnvironment;
        }

        // GET api/recettes/5
        [HttpGet]
        public dynamic Get([FromQuery] String id)
        {
            if (String.IsNullOrEmpty(id))
            {
                var recettes = _recettesRepository.Get();
                recettes.ToList().ForEach(r => r.Calories = r.IngredientsRecettes.Sum(i => i.Ingredient.Calories));
                return recettes.ToList();
            }
            else
            {
                var recette = _recettesRepository.Get().Where(r => r.Id == id).FirstOrDefault();
                recette.Calories = recette.IngredientsRecettes.Sum(i => i.Ingredient.Calories);
                return recette;
            }
        }

        [HttpPost]
        public Recette Post([FromBody]Recette recipe)
        {
            recipe.Id = recipe.Name.Replace(" ", "-").ToLower();

            if (_recettesRepository.Get().Any(r => r.Id == recipe.Id))
                recipe.Id = String.Format("{0}-{1}", recipe.Id, Guid.NewGuid());

            recipe.IsAvailable = true;
            //Un nom est généré aléatoirement afin de ne pas avoir à se soucier des collisions avec d'autres images sur le file system
            //Gère uniquement les jpg
            recipe.Picture = String.Format("img/recettes/{0}.jpg", Guid.NewGuid().ToString());

            _recettesRepository.Add(recipe);

            if (recipe.IngredientsRecettes == null)
                recipe.IngredientsRecettes = new List<IngredientRecette>();

            //Pas propre
            if (recipe.Ingredients != null && recipe.Ingredients.Count > 0)
                recipe.Ingredients = recipe.Ingredients[0].Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

            foreach (String ingredientId in recipe.Ingredients)
            {
                Ingredient ig = _ingredientsRepository.Get().Where(i => i.Id == ingredientId).FirstOrDefault();

                if (ig != null)
                    recipe.IngredientsRecettes.Add(new IngredientRecette { IngredientId = ig.Id, RecetteId = recipe.Id });
            }

            #region Image facultative

            if (Request.HasFormContentType)
            {
                if (Request.Form.Files["rawPicture"] != null)
                {
                    byte[] imageBinary = new byte[Request.Form.Files["rawPicture"].Length];

                    using (System.IO.Stream fileStream = Request.Form.Files["rawPicture"].OpenReadStream())
                        fileStream.Read(imageBinary, 0, imageBinary.Length);

                    String pictureFullPath = String.Format("{0}/{1}", _appEnvironment.WebRootPath, recipe.Picture);

                    using (System.IO.FileStream fs = new System.IO.FileStream(pictureFullPath, System.IO.FileMode.Create))
                        fs.Write(imageBinary, 0, imageBinary.Length);
                }
            }

            #endregion

            return recipe;
        }

        [HttpPost]
        [Route("setimage")]
        public void SetImage([FromBody] String id)
        {
            if (Request.Form.Files.Count == 0 || Request.Form.Files["rawPicture"] == null)
                return;

            var recipe = _recettesRepository.Get().Where(r => r.Id == id)
                    .FirstOrDefault();

            byte[] imageBinary = new byte[Request.Form.Files["rawPicture"].Length];

            using (System.IO.Stream fileStream = Request.Form.Files["rawPicture"].OpenReadStream())
                fileStream.Read(imageBinary, 0, imageBinary.Length);

            String pictureFullPath = String.Format("{0}/{1}", _appEnvironment.WebRootPath, recipe.Picture);

            using (System.IO.FileStream fs = new System.IO.FileStream(pictureFullPath, System.IO.FileMode.Create))
                fs.Write(imageBinary, 0, imageBinary.Length);
        }
    }
}
