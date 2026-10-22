import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function minDateValidator(getMin: () => string): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value as string;
    if (!value) return null;
    const min = getMin();
    return value < min ? { minDate: { min, actual: value } } : null;
  };
}