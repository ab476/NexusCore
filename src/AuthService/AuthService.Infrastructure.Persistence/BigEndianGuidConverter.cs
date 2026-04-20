using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace NC.AuthService.Infrastructure.Persistence;

public class BigEndianGuidConverter : ValueConverter<Guid, byte[]>
{
    public BigEndianGuidConverter()
        : base(
            v => v.ToByteArray(bigEndian: true),
            v => new Guid(v, bigEndian: true)
        )
    {
    }
}