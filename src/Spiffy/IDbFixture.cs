using System.Data;

namespace Spiffy {
	public interface IDbFixture {
		IDbConnection NewConnection();
	}
}
