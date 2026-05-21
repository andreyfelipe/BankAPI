export interface RegisterRequest {
  name: string;
  email: string;
  password: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  name: string;
  email: string;
  expiresAt: string;
}

export interface RegisterResponse {
  userId: string;
  name: string;
  email: string;
}
