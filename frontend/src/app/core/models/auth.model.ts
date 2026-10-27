import { Location } from './location.model';

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  email: string;
  locations: Location[];
}

export interface Session {
  token: string;
  email: string;
}
