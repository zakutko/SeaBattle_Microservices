export interface User
{
    username: string;
    token: string;
}

export interface UserFormValues
{
    email: string;
    password: string;
    displayName?: string;
    userName?: string;
}