import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function passwordMatchValidator(otherControlName: string): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const other = control.parent?.get(otherControlName);

    if (!other || !control.value) {
      return null;
    }

    return control.value === other.value ? null : { passwordMismatch: true };
  };
}