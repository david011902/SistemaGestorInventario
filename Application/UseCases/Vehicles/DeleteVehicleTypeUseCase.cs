using Domain.Abstractions;


namespace Application.UseCases.Vehicles
{
    public class DeleteVehicleTypeUseCase
    {
        private readonly IVehicleTypeRepository _repository;
        public DeleteVehicleTypeUseCase(IVehicleTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task ExecuteAsync(Guid id)
        {
            var vehicleType = await _repository.GetByIdAsync(id);
            if (vehicleType == null)
            {
                throw new InvalidOperationException($"No se encontro el tipo de vehiculo con el ID: {id} ");
            }
            vehicleType.Desactivate();
            await _repository.UpdateAsync(vehicleType);
            await _repository.SaveChangesAsync();
        }
    }
}
