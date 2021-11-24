namespace LibYear.Core.FileTypes;

public interface IProjectFile
{
	string FileName { get; }
	IDictionary<string, PackageVersion?> Packages { get; }
	string Update(IEnumerable<Result> results);
}