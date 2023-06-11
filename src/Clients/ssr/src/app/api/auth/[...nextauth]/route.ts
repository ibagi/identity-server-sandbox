import NextAuth, { TokenSet, User } from "next-auth"
import DuendeIDS6Provider from "next-auth/providers/duende-identity-server6"

const issuer = process.env.DUENDE_IDS6_ISSUER || 'http://localhost:5273'

type UserProfile = {
  name: string;
  given_name: string;
  email: string;
}

async function fetchUserProfile(tokens: TokenSet): Promise<UserProfile> {
  const request = await fetch(`${issuer}/connect/userinfo`, {
    headers: {
      'Authorization': `Bearer ${tokens.access_token}`
    }
  })

  const profile = await request.json();
  return profile as UserProfile;
}

const handler = NextAuth({
  providers: [
    DuendeIDS6Provider({
        clientId: process.env.DUENDE_IDS6_ID || 'ssr-client',
        clientSecret: process.env.DUENDE_IDS6_SECRET || 'ssr-client-secret',
        issuer: issuer,
        async profile(oauthProfile, tokens) {
          console.log(oauthProfile)
          const profile = await fetchUserProfile(tokens);

          return {
            id: oauthProfile.sub,
            name: profile.name,
            email: profile.email,
            image: null,
          }
        }
      })
  ]
})

export { handler as GET, handler as POST }