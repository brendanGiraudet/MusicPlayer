namespace MusicPlayerApplication.Stores.Actions;

public record RemoveSongResultAction(bool IsSuccess, string Filename){}