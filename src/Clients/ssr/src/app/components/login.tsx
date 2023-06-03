"use client";

import { useSession, signIn, signOut } from "next-auth/react"

type SignOutFn = typeof signOut;

function SignOutButton({ email, signOut }: { email: string, signOut: SignOutFn }) {
    return (
        <>
            <div>Signed in as {email} </div>
            <button className="btn btn-primary" onClick={() => signOut()}>Sign out</button>
        </>
    )
}

function SignInButton({ signIn }: { signIn: () => void }) {
    return <button className="btn btn-primary" onClick={signIn}>Sign in</button>
}

export default function LoginSection() {
    const session = useSession()
    const button = session.data?.user?.email
        ? <SignOutButton email={session.data.user.email} signOut={signOut} />
        : <SignInButton signIn={() => signIn('duende-identityserver6')} />

    return (
        <div className="hero min-h-screen bg-base-200">
            <div className="hero-content text-center">
                <div className="max-w-md">
                    <h1 className="text-5xl font-bold">Local login</h1>
                    <p className="py-6">You can login or logout with your local user</p>
                    {button}
                </div>
            </div>
        </div>
    )
}