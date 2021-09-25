using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Utils.Services.Interfaces;

namespace NomaNova.Ojeda.Data.Seeders
{
    public static class FieldsSeeder
    {
        private static readonly List<Field> Fields = new()
        {
            new()
            {
                Id = "f117ef9d-5c11-4478-8db0-72ec8d481121",
                Name = "Model",
                Description = "Type of the device, e.g. 'iPhone XR' or 'Inspiron 15 3000'",
                Type = FieldType.Text
            },
            new()
            {
                Id = "676aca2c-5a2b-4e07-96fc-15eba5652597",
                Name = "Manufacturer",
                Description = "Also 'make' or 'brand', e.g. 'Apple' or 'Dell'",
                Type = FieldType.Text
            },
            new()
            {
                Id = "8eec129f-adb8-4023-8bc2-fd1060f648a4",
                Name = "Serial Number",
                Description = "Unique device identification as provided by the manufacturer",
                Type = FieldType.Text
            },
            new()
            {
                Id = "3287cdee-c840-4b1a-9883-63c5e6b7827d",
                Name = "Vendor",
                Description = "Name of the vendor or store where the device was purchased",
                Type = FieldType.Text
            },
            new()
            {
                Id = "1e0ea3d8-4120-491c-a23c-e88547023ccd",
                Name = "PO Number",
                Description = "Purchase order number, as provided by accounting",
                Type = FieldType.Text
            },
            new()
            {
                Id = "25de2aee-af9f-4507-835c-59b9af132cde",
                Name = "Processor (CPU)",
                Description = "e.g. '2,3 GHz 8-Core Intel Core i9'",
                Type = FieldType.Text
            },
            new()
            {
                Id = "57ce77af-f4e7-49d2-b7ac-84cb68ee0d05",
                Name = "Memory (RAM)",
                Description = "e.g. '32 GB 2667 MHz DDR4'",
                Type = FieldType.Text
            },
            new()
            {
                Id = "810be27f-6c60-4ba7-b23e-8507ebab979c",
                Name = "Support Contract Number",
                Description = "",
                Type = FieldType.Text
            },
            new()
            {
                Id = "2d8fe7a8-56e6-48e1-ab7c-953aaea4dbbc",
                Name = "Support Phone Number",
                Description = "",
                Type = FieldType.Text
            },
            new()
            {
                Id = "5a00bc50-4f7e-4c69-8bd7-fc848e0fa635",
                Name = "Support Email Address",
                Description = "",
                Type = FieldType.Text
            },
            new()
            {
                Id = "eb939ef1-3bed-477e-be29-93b5d7be8453",
                Name = "Operating System (OS)",
                Description = "e.g. 'macOS Catalina 10.15'",
                Type = FieldType.Text
            },
            new()
            {
                Id = "e740f79d-c3a4-40bd-ab8e-96d2bf1aa70f",
                Name = "IMEI",
                Description = "Shown by entering '*#06#' on the dialpad",
                Type = FieldType.Text
            },
            new()
            {
                Id = "1b4c8b48-cd42-4ae3-8663-d53be51dca8e",
                Name = "Graphics (Card)",
                Description = "e.g. 'AMD Radeon Pro 5500M 8 GB'",
                Type = FieldType.Text
            },
            new()
            {
                Id = "59c0e320-a828-4793-8289-74fa1161a6bc",
                Name = "Screen",
                Description = "e.g. '16-inch (3072 x 1920)'",
                Type = FieldType.Text
            }
        };

        public static async Task Seed(DbContext context, ITimeKeeper timeKeeper)
        {
            foreach (var field in Fields)
            {
                await UpsertField(context, timeKeeper, field);
            }
        }

        private static async Task UpsertField(DbContext context, ITimeKeeper timeKeeper, Field field)
        {
            var dbField = await context.Set<Field>().FirstOrDefaultAsync(f => f.Name.Equals(field.Name));
            var now = timeKeeper.UtcNow;

            if (dbField == null)
            {
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