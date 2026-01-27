using MaxApiMy.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxApiMy.Common
{
	public static class AttachmentHelper
	{
		public static Attachment CreateImageAttachment(this string photoUrl)
		{
			return new PhotoAttachment
			{
				Type = AttachmentType.image,
				Payload = new PhotoAttachmentPayload { Url = photoUrl }
			};
		}
		/*
		public static Attachment CreateVideoAttachment(this string token)
		{
			return new Attachment
			{
				Type = AttachmentType.video,
				Payload = new UploadedInfo { Token = token }
			};
		}
		*/
	}
}
