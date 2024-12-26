using System.Threading.Tasks;
using Fluxor;
using Microsoft.Extensions.Logging;
using MusicPlayerApplication.Services.ModalService;
using MusicPlayerApplication.Services.SongService;
using MusicPlayerApplication.Stores.Actions;

namespace MusicPlayerApplication.Stores;

public class MusicsEffects
(
    ISongService _songService,
    ILogger<MusicsEffects> logger,
    IModalService _modalService
)
{
    [EffectMethod]
    public async Task HandleGetSongsAction(GetSongsAction action, IDispatcher dispatcher)
    {
        var getSongsResponse = await _songService.GetSongsAsync();

        if (getSongsResponse.HasError)
        {
            var message = "Erreur lors du chargement des musiques";
            await _modalService.ShowAsync(message, getSongsResponse.ErrorMessage);

            logger.LogError(message);
        }

        dispatcher.Dispatch(new GetSongsResultAction(getSongsResponse.Content));
    }


    [EffectMethod]
    public async Task HandleRemoveSongAction(RemoveSongAction action, IDispatcher dispatcher)
    {
        var result = await _songService.RemoveByNameAsync(action.Filename);
        await _modalService.ShowAsync("Confirmation", $"La musique {action.Title}({action.Filename}) à bien été supprimée");
        
        dispatcher.Dispatch(new RemoveSongResultAction(!result.HasError, action.Filename));
    }
}