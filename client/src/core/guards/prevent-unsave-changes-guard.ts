import { CanDeactivateFn } from '@angular/router';
import { MemberProfile } from '../../features/members/member-profile/member-profile';

export const preventUnsaveChangesGuard: CanDeactivateFn<MemberProfile> = (component, currentRoute, currentState, nextState) => {
  if (component.editForm?.dirty) {
    return confirm('Unsaved changes. Do  you want to continue?');
  }

  return true;
};
