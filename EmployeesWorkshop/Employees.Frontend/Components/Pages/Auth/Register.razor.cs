using Employees.Frontend.Repositories;
using Employees.Frontend.Services;
using Employees.Shared.DTOs;
using Employees.Shared.Entities;
using Employees.Shared.Enums;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Employees.Frontend.Components.Pages.Auth;

public partial class Register
{
    private UserDTO userDTO = new();

    // Listas inicializadas para evitar null
    private List<Country> countries = new();

    private List<State> states = new();
    private List<City> cities = new();

    private bool loading;
    private string? imageUrl;
    private string? titleLabel;

    private Country? selectedCountry;
    private State? selectedState;
    private City? selectedCity;

    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ILoginService LoginService { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IRepository Repository { get; set; } = null!;
    [Parameter, SupplyParameterFromQuery] public bool IsAdmin { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadCountriesAsync();
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        titleLabel = IsAdmin ? "Registro de Administrador" : "Registro de Usuario";
    }

    private void ImageSelected(string imageBase64)
    {
        userDTO.Photo = imageBase64;
        imageUrl = null;
    }

    private async Task LoadCountriesAsync()
    {
        var responseHttp = await Repository.GetAsync<List<Country>>("/api/countries/combo");
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(message!, Severity.Error);
            return;
        }

        countries = responseHttp.Response ?? new List<Country>();
    }

    private async Task LoadStatesAsync(int countryId)
    {
        var responseHttp = await Repository.GetAsync<List<State>>($"/api/states/combo/{countryId}");
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(message!, Severity.Error);
            return;
        }

        states = responseHttp.Response ?? new List<State>();
    }

    private async Task LoadCitiesAsync(int stateId)
    {
        var responseHttp = await Repository.GetAsync<List<City>>($"/api/cities/combo/{stateId}");
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(message!, Severity.Error);
            return;
        }

        cities = responseHttp.Response ?? new List<City>();
    }

    private async Task CountryChangedAsync(Country country)
    {
        selectedCountry = country;
        selectedState = null;
        selectedCity = null;
        states = new List<State>();
        cities = new List<City>();

        await LoadStatesAsync(country.Id);
    }

    private async Task StateChangedAsync(State state)
    {
        selectedState = state;
        selectedCity = null;
        cities = new List<City>();

        await LoadCitiesAsync(state.Id);
    }

    private void CityChanged(City city)
    {
        selectedCity = city;
        userDTO.CityId = city.Id;
    }

    // ========= BUSCADORES PARA LOS AUTOCOMPLETE ==========

    private Task<IEnumerable<Country>> SearchCountries(string searchText, CancellationToken token)
    {
        IEnumerable<Country> query = countries;

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            query = query.Where(c =>
                c.FirstName.Contains(searchText, StringComparison.InvariantCultureIgnoreCase));
        }

        return Task.FromResult(query);
    }

    private Task<IEnumerable<State>> SearchStates(string searchText, CancellationToken token)
    {
        IEnumerable<State> query = states;

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            query = query.Where(s =>
                s.FirstName.Contains(searchText, StringComparison.InvariantCultureIgnoreCase));
        }

        return Task.FromResult(query);
    }

    private Task<IEnumerable<City>> SearchCity(string searchText, CancellationToken token)
    {
        IEnumerable<City> query = cities;

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            query = query.Where(c =>
                c.FirstName.Contains(searchText, StringComparison.InvariantCultureIgnoreCase));
        }

        return Task.FromResult(query);
    }

    // =====================================================

    private void ReturnAction()
    {
        NavigationManager.NavigateTo("/");
    }

    private void InvalidForm()
    {
        Snackbar.Add("Por favor llena todos los campos del formulario.", Severity.Warning);
    }

    private async Task CreateUserAsync()
    {
        if (userDTO.Email is null || userDTO.PhoneNumber is null || userDTO.CityId == 0)
        {
            InvalidForm();
            return;
        }

        userDTO.UserType = IsAdmin ? UserType.Admin : UserType.User;
        userDTO.UserName = userDTO.Email;

        loading = true;
        var responseHttp = await Repository.PostAsync<UserDTO, TokenDTO>("/api/accounts/CreateUser", userDTO);
        loading = false;

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(message!, Severity.Error);
            return;
        }

        await LoginService.LoginAsync(responseHttp.Response!.Token);
        NavigationManager.NavigateTo("/");
    }
}