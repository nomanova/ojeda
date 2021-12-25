namespace NomaNova.Ojeda.Models.Dtos.AssetIds;

public class GenerateAssetIdDto
{
    /**
     * Minimal representation of the asset id.
     * Excludes any padding, formatting or calculated check digit(s).
     * Used when the asset id is edited.
     */
    public string AssetId { get; set; }
    
    /**
     * Full representation of the asset id.
     * Includes padding, formatting and check digit(s), when applicable.
     * Used when the asset id is presented.
     */
    public string FullAssetId { get; set; }
}