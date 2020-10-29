import { Media } from './media';
import { User } from './user';

export interface UserMedias {
    appUser: User;
    medias: Media[];
    countMedias: number;
    countAbonne: number;
    countAbonnement: number;
}