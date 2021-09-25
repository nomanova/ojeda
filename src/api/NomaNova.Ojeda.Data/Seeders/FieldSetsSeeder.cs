using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NomaNova.Ojeda.Core.Domain.FieldSets;
using NomaNova.Ojeda.Utils.Services.Interfaces;

namespace NomaNova.Ojeda.Data.Seeders
{
    public static class FieldSetsSeeder
    {
        public static async Task Seed(DbContext context, ITimeKeeper timeKeeper)
        {
            var fieldSets = new List<FieldSet>();
            
            var supportFieldSet = new FieldSet
            {
                Id = "4aeccbc8-f0a2-42d5-a28c-11bbc891ee63",
                Name = "Support",
                Description = "Support contact details",
                FieldSetFields = new List<FieldSetField>
                {
                    new() // Support Email
                    {
                        FieldId = "5a00bc50-4f7e-4c69-8bd7-fc848e0fa635",
                        FieldSetId = "4aeccbc8-f0a2-42d5-a28c-11bbc891ee63",
                        Order = 1
                    },
                    new() // Support Phone
                    {
                        FieldId = "2d8fe7a8-56e6-48e1-ab7c-953aaea4dbbc",
                        FieldSetId = "4aeccbc8-f0a2-42d5-a28c-11bbc891ee63",
                        Order = 2
                    },
                    new() // Support Number
                    {
                        FieldId = "810be27f-6c60-4ba7-b23e-8507ebab979c",
                        FieldSetId = "4aeccbc8-f0a2-42d5-a28c-11bbc891ee63",
                        Order = 3
                    }
                }
            };
            fieldSets.Add(supportFieldSet);
            
            foreach (var fieldSet in fieldSets)
            {
                await UpsertFieldSet(context, timeKeeper, fieldSet);
            }
        }

        private static async Task UpsertFieldSet(DbContext context, ITimeKeeper timeKeeper, FieldSet fieldSet)
        {
            var dbFieldSet = await context.Set<FieldSet>().FirstOrDefaultAsync(f => f.Name.Equals(fieldSet.Name));
            var now = timeKeeper.UtcNow;

            if (dbFieldSet == null)
            {
                fieldSet.CreatedAt = now;
                fieldSet.UpdatedAt = now;
                
                await context.Set<FieldSet>().AddAsync(fieldSet);
            }
            else
            {
                dbFieldSet.Description = fieldSet.Description;

                dbFieldSet.UpdatedAt = now;
                
                context.Set<FieldSet>().Update(dbFieldSet);
            }
            
            await context.SaveChangesAsync();
        }
    }
}