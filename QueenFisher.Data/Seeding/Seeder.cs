using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using QueenFisher.Data.Context;
using QueenFisher.Data.Domains;

namespace QueenFisher.Data.Seeding
{
    public class Seeder
    {

        public static async Task SeedData(IApplicationBuilder app)
        {
            //Get db context
            var dbContext = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<QueenFisherDbContext>();

            if (dbContext.Database.GetPendingMigrations().Any())
            {
                dbContext.Database.Migrate();
            }
            if (dbContext.Users.Any())
            {
                return;
            }
            else
            {
                var baseDir = Directory.GetCurrentDirectory();

                await dbContext.Database.EnsureCreatedAsync();
                //Get Usermanager and rolemanager from IoC container
                var userManager = app.ApplicationServices.CreateScope()
                                              .ServiceProvider.GetRequiredService<UserManager<AppUser>>();

                var roleManager = app.ApplicationServices.CreateScope()
                                                .ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                //Creating list of roles

                List<string> roles = new() { "SuperAdmin", "Admin", "Customer" };

                //Creating roles
                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(new IdentityRole { Name = role });
                }
                //Instantiating i User and it properties
                var user = new AppUser
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = "Chidi",
                    LastName = "SuperAdmin",
                    UserName = "Michael",
                    Email = "super@queenfisher.com",
                    PhoneNumber = "08162292349",
                    PhoneNumberConfirmed = true,
                    Gender = Enums.Gender.Male,
                    IsActive = true,
                    PublicId = null,
                    Avatar = "http://placehold.it/32x32",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    EmailConfirmed = true

                };
                await userManager.CreateAsync(user, "Password@123");
                await userManager.AddToRoleAsync(user, roles[0]);


                //Creating file path for each model class
                var path = File.ReadAllText(FilePath(baseDir, "JsonFiles/Users.json"));
                var timeTablePath = File.ReadAllText(FilePath(baseDir, "JsonFiles/TimeTable.json"));
                var mealPath = File.ReadAllText(FilePath(baseDir, "JsonFiles/Meal.json"));
                var recipePath = File.ReadAllText(FilePath(baseDir, "JsonFiles/Recipe.json"));
                var ingredientPath = File.ReadAllText(FilePath(baseDir, "JsonFiles/Ingredient.json"));
                var photoPath = File.ReadAllText(FilePath(baseDir, "JsonFiles/Photo.json"));
                var videoPath = File.ReadAllText(FilePath(baseDir, "JsonFiles/Video.json"));

                //Converting the JSON objects to their respective class objects
                var hbaUsers = JsonConvert.DeserializeObject<List<AppUser>>(path);
                var hbaTimeTable = JsonConvert.DeserializeObject<List<TimeTable>>(timeTablePath);
                var hbaMeal = JsonConvert.DeserializeObject<List<Meal>>(mealPath);
                var hbaRecipe = JsonConvert.DeserializeObject<List<Recipe>>(recipePath);
                var hbaIngredient = JsonConvert.DeserializeObject<List<Ingredient>>(ingredientPath);
                var hbaPhoto = JsonConvert.DeserializeObject<List<Photo>>(photoPath);
                var hbaVideo = JsonConvert.DeserializeObject<List<Video>>(videoPath);


                //Seeding Users
                for (int i = 0; i < hbaUsers.Count; i++)
                {
                    hbaUsers[i].EmailConfirmed = true;
                    await userManager.CreateAsync(hbaUsers[i], "Password@123");

                    //Making the first five users to be Admins
                    if (i < 5)
                    {
                        await userManager.AddToRoleAsync(hbaUsers[i], roles[1]);
                        continue;
                    }
                    await userManager.AddToRoleAsync(hbaUsers[i], roles[2]);
                }

                //Seeding Timetable
                for (int i = 0; i < hbaTimeTable.Count; i++)
                {
                    var timeTable = new TimeTable
                    {
                        Id = hbaTimeTable[i].Id,
                        MealType = hbaTimeTable[i].MealType,
                        StartDate = hbaTimeTable[i].StartDate,
                        EndDate = hbaTimeTable[i].EndDate,
                        AppUserId = hbaTimeTable[i].AppUserId
                    };
                    await dbContext.TimeTables.AddAsync(timeTable);
                }

                //Seeding Meals
                for (int i = 0; i < hbaMeal.Count; i++)
                {
                    var meal = new Meal
                    {
                        Id = hbaMeal[i].Id,
                        Name = hbaMeal[i].Name,
                        TimeTableId = hbaMeal[i].TimeTableId,
                        CreatedAt = hbaMeal[i].CreatedAt,
                        UpdatedAt = hbaMeal[i].UpdatedAt,
                        IsDeleted = hbaMeal[i].IsDeleted
                    };
                    await dbContext.Meals.AddAsync(meal);
                }

                //Seeding Recipes
                for (int i = 0; i < hbaRecipe.Count; i++)
                {
                    var recipe = new Recipe
                    {
                        Id = hbaRecipe[i].Id,
                        Name = hbaRecipe[i].Name,
                        MealType = hbaRecipe[i].MealType,
                        Procedure = hbaRecipe[i].Procedure,
                        Summary = hbaRecipe[i].Summary,
                        AppUserId = hbaRecipe[i].AppUserId,
                        IsDeleted = false
                    };
                    await dbContext.Recipes.AddAsync(recipe);
                }

                //Seeding Ingredients
                for (int i = 0; i < hbaIngredient.Count; i++)
                {
                    var ingredient = new Ingredient
                    {
                        Id = hbaIngredient[i].Id,
                        Name = hbaIngredient[i].Name,
                        RecipeId = hbaIngredient[i].RecipeId,
                        CreatedAt = hbaIngredient[i].CreatedAt,
                        UpdatedAt = hbaIngredient[i].UpdatedAt,
                        IsDeleted = false
                    };
                }

                //Seeding Photos
                for (int i = 0; i < hbaPhoto.Count; i++)
                {
                    var photo = new Photo
                    {
                        Id = hbaPhoto[i].Id,
                        Url = hbaPhoto[i].Url,
                        IsMain = hbaPhoto[i].IsMain,
                        PublicId = hbaPhoto[i].PublicId,
                        RecipeId = hbaPhoto[i].RecipeId,
                    };
                }

                //Seeding Videos
                for (int i = 0; i < hbaVideo.Count; i++)
                {
                    var video = new Video
                    {
                        Id = hbaVideo[i].Id,
                        Url = hbaVideo[i].Url,
                        IsMain = hbaVideo[i].IsMain,
                        PublicId = hbaVideo[i].PublicId,
                        RecipeId = hbaVideo[i].RecipeId,
                    };
                }

            }
            //Saving everything into the database
            await dbContext.SaveChangesAsync();
        }

        //Defining method to get file paths
        static string FilePath(string folderName, string fileName)
        {
            return Path.Combine(folderName, fileName);
        }
    }
}
