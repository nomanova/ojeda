using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NomaNova.Ojeda.Api.Tests.Builders;
using NomaNova.Ojeda.Core.Domain.Assets;
using NomaNova.Ojeda.Core.Domain.AssetTypes;
using NomaNova.Ojeda.Core.Domain.Fields;
using NomaNova.Ojeda.Core.Domain.FieldSets;

namespace NomaNova.Ojeda.Api.Tests.Fixtures
{
    /**
     * A default fixture including 4 fields, 3 field sets, 2 asset types and 4 assets.
     * All assets have a value for each field.
     */
    public class DefaultAssetFixture
    {
        public Field Field1 { get; private set; }

        public Field Field2 { get; private set; }

        public Field Field3 { get; private set; }

        public Field Field4 { get; private set; }

        public FieldSet FieldSet1 { get; private set; }

        public FieldSet FieldSet2 { get; private set; }

        public FieldSet FieldSet3 { get; private set; }

        public AssetType AssetType1 { get; private set; }

        public AssetType AssetType2 { get; private set; }

        public Asset Asset1 { get; private set; }

        public Asset Asset2 { get; private set; }

        public Asset Asset3 { get; private set; }

        public Asset Asset4 { get; private set; }

        public static async Task<DefaultAssetFixture> Create(DbContext databaseContext)
        {
            var fixture = new DefaultAssetFixture();

            // Fields
            fixture.Field1 = await new FieldBuilder()
                .WithName("Field 1")
                .Build(databaseContext);

            fixture.Field2 = await new FieldBuilder()
                .WithName("Field 2")
                .Build(databaseContext);

            fixture.Field3 = await new FieldBuilder()
                .WithName("Field 3")
                .Build(databaseContext);

            fixture.Field4 = await new FieldBuilder()
                .WithName("Field 4")
                .Build(databaseContext);

            // Field Sets
            fixture.FieldSet1 = await new FieldSetBuilder()
                .WithName("Field Set 1")
                .WithFields(fixture.Field1, fixture.Field2)
                .Build(databaseContext);

            fixture.FieldSet2 = await new FieldSetBuilder()
                .WithName("Field Set 2")
                .WithFields(fixture.Field2, fixture.Field3)
                .Build(databaseContext);

            fixture.FieldSet3 = await new FieldSetBuilder()
                .WithName("Field Set 3")
                .WithFields(fixture.Field3, fixture.Field4)
                .Build(databaseContext);

            // Asset Types
            fixture.AssetType1 = await new AssetTypeBuilder()
                .WithName("Asset Type 1")
                .WithFieldSets(fixture.FieldSet1, fixture.FieldSet2)
                .Build(databaseContext);

            fixture.AssetType2 = await new AssetTypeBuilder()
                .WithName("Asset Type 2")
                .WithFieldSets(fixture.FieldSet2, fixture.FieldSet3)
                .Build(databaseContext);

            // Assets
            fixture.Asset1 = await new AssetBuilder(fixture.AssetType1.Id)
                .WithName("Asset 1")
                .WithFieldValue(fixture.Field1, fixture.FieldSet1, "Field 1 - FieldSet 1 - Asset 1")
                .WithFieldValue(fixture.Field2, fixture.FieldSet1, "Field 2 - FieldSet 1 - Asset 1")
                .WithFieldValue(fixture.Field2, fixture.FieldSet2, "Field 2 - FieldSet 2 - Asset 1")
                .WithFieldValue(fixture.Field3, fixture.FieldSet2, "Field 3 - FieldSet 2 - Asset 1")
                .Build(databaseContext);

            fixture.Asset2 = await new AssetBuilder(fixture.AssetType1.Id)
                .WithName("Asset 2")
                .WithFieldValue(fixture.Field1, fixture.FieldSet1, "Field 1 - FieldSet 1 - Asset 2")
                .WithFieldValue(fixture.Field2, fixture.FieldSet1, "Field 2 - FieldSet 1 - Asset 2")
                .WithFieldValue(fixture.Field2, fixture.FieldSet2, "Field 2 - FieldSet 2 - Asset 2")
                .WithFieldValue(fixture.Field3, fixture.FieldSet2, "Field 3 - FieldSet 2 - Asset 2")
                .Build(databaseContext);

            fixture.Asset3 = await new AssetBuilder(fixture.AssetType2.Id)
                .WithName("Asset 3")
                .WithFieldValue(fixture.Field2, fixture.FieldSet2, "Field 2 - FieldSet 2 - Asset 3")
                .WithFieldValue(fixture.Field3, fixture.FieldSet2, "Field 3 - FieldSet 2 - Asset 3")
                .WithFieldValue(fixture.Field3, fixture.FieldSet3, "Field 3 - FieldSet 3 - Asset 3")
                .WithFieldValue(fixture.Field4, fixture.FieldSet3, "Field 4 - FieldSet 3 - Asset 3")
                .Build(databaseContext);

            fixture.Asset4 = await new AssetBuilder(fixture.AssetType2.Id)
                .WithName("Asset 4")
                .WithFieldValue(fixture.Field2, fixture.FieldSet2, "Field 2 - FieldSet 2 - Asset 4")
                .WithFieldValue(fixture.Field3, fixture.FieldSet2, "Field 3 - FieldSet 2 - Asset 4")
                .WithFieldValue(fixture.Field3, fixture.FieldSet3, "Field 3 - FieldSet 3 - Asset 4")
                .WithFieldValue(fixture.Field4, fixture.FieldSet3, "Field 4 - FieldSet 3 - Asset 4")
                .Build(databaseContext);

            return fixture;
        }

        private DefaultAssetFixture()
        {
        }
    }
}