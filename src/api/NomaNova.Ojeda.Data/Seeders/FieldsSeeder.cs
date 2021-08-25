using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Core.Helpers.Interfaces;

namespace NomaNova.Ojeda.Data.Seeders
{
    public static class FieldsSeeder
    {
        public static async Task Seed(DbContext context, ITimeKeeper timeKeeper)
        {
            var fields = new List<Field>
            {
                new()
                {
                    Name = "Model",
                    Description = "Type of the device, e.g. 'iPhone XR' or 'Inspiron 15 3000'",
                    Type = FieldType.Text
                },
                new ()
                {
                    Name = "Manufacturer",
                    Description = "Also 'make' or 'brand', e.g. 'Apple' or 'Dell'",
                    Type = FieldType.Text
                },
                new ()
                {
                    Name = "Serial Number",
                    Description = "Unique device identification as provided by the manufacturer",
                    Type = FieldType.Text
                },
                new ()
                {
                    Name = "Vendor",
                    Description = "Name of the vendor or store where the device was purchased",
                    Type = FieldType.Text
                },
                new ()
                {
                    Name = "PO Number",
                    Description = "Purchase order number, as provided by accounting",
                    Type = FieldType.Text
                },
                new ()
                {
                    Name = "Processor (CPU)",
                    Description = "e.g. '2,3 GHz 8-Core Intel Core i9'",
                    Type = FieldType.Text
                },
                new ()
                {
                    Name = "Memory (RAM)",
                    Description = "e.g. '32 GB 2667 MHz DDR4'",
                    Type = FieldType.Text
                },
                new ()
                {
                    Name = "Support Contract Number",
                    Description = "",
                    Type = FieldType.Text
                },
                new ()
                {
                    Name = "Support Phone Number",
                    Description = "",
                    Type = FieldType.Text
                },
                new ()
                {
                    Name = "Support Email Address",
                    Description = "",
                    Type = FieldType.Text
                },
                new ()
                {
                    Name = "Operating System (OS)",
                    Description = "e.g. 'macOS Catalina 10.15'",
                    Type = FieldType.Text
                },
                new ()
                {
                    Name = "IMEI",
                    Description = "Shown by entering '*#06#' on the dialpad",
                    Type = FieldType.Text
                },
                new ()
                {
                    Name = "Graphics (Card)",
                    Description = "e.g. 'AMD Radeon Pro 5500M 8 GB'",
                    Type = FieldType.Text
                },
                new ()
                {
                    Name = "Screen",
                    Description = "e.g. '16-inch (3072 x 1920)'",
                    Type = FieldType.Text
                }
            };

            foreach (var field in fields)
            {
                await UpsertField(context, timeKeeper, field);
            }
        }

        private static async Task UpsertField(DbContext context,ITimeKeeper timeKeeper, Field field)
        {
            var dbField = await context.Set<Field>().FirstOrDefaultAsync(f => f.Name.Equals(field.Name));
            var now = timeKeeper.UtcNow;
            
            if (dbField == null)
            {
                field.Id = Guid.NewGuid().ToString();
                field.CreatedAt = now;
                field.UpdatedAt = now;
                
                await context.Set<Field>().AddAsync(field);
            }
            else
            {
                dbField.Description = field.Description;
                dbField.Type = field.Type;
                
                dbField.UpdatedAt = now;
                
                context.Set<Field>().Update(dbField);
            }
            
            await context.SaveChangesAsync();
        }
    }
}