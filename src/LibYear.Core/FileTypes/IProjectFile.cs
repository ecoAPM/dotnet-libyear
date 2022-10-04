namespace LibYear.Core.FileTypes;

public interface IProjectFile
{
	string FileName { get; }
	IDictionary<string, PackageVersion?> Packages { get; }
	string Update(IReadOnlyCollection<Result> results);
}